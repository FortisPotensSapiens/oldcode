import { BoxProps } from '@mui/system';
import { DragEventHandler, useCallback, useEffect, useState } from 'react';

import { preventEvent } from './preventEvent';
import { ChangeHandler } from './types';

type UseDragAndDrop = (
  files: File[],
  onChange: ChangeHandler,
  disabled?: boolean,
) => [boolean, Pick<BoxProps<'label'>, 'onDragEnter' | 'onDragLeave' | 'onDrop' | 'onDragOver'>];

const useDragAndDrop: UseDragAndDrop = (files, onChange, disabled) => {
  const [state, setState] = useState(false);

  useEffect(() => {
    if (disabled) {
      setState(false);
    }
  }, [disabled]);

  const onDragOver = useCallback<DragEventHandler<HTMLLabelElement>>(
    (event) => {
      preventEvent(event);

      if (!disabled) {
        setState(true);
      }
    },
    [disabled],
  );

  const onDrop = useCallback<DragEventHandler<HTMLLabelElement>>(
    (event) => {
      preventEvent(event);
      setState(false);

      if (disabled || event.dataTransfer.files.length === 0) {
        return;
      }

      onChange([...files, ...Array.from(event.dataTransfer.files)]);
    },
    [files, onChange, disabled],
  );

  const onDragEnter = useCallback<DragEventHandler<HTMLLabelElement>>(
    (event) => {
      preventEvent(event);

      if (!disabled) {
        setState(true);
      }
    },
    [disabled],
  );

  const onDragLeave = useCallback<DragEventHandler<HTMLLabelElement>>(
    (event) => {
      preventEvent(event);

      if (!disabled) {
        setState(false);
      }
    },
    [disabled],
  );

  return [state, { onDragEnter, onDragLeave, onDragOver, onDrop }];
};

export { useDragAndDrop };
export type { UseDragAndDrop };
