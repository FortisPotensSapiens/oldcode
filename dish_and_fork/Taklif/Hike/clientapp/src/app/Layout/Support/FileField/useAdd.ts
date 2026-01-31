import { ChangeEventHandler, useCallback } from 'react';

import { ChangeHandler } from './types';

type UseAdd = (files: File[], onAdd: ChangeHandler) => ChangeEventHandler<HTMLInputElement>;

const useAdd: UseAdd = (files, onChange) =>
  useCallback<ChangeEventHandler<HTMLInputElement>>(
    (event) => {
      if (!event.currentTarget.files) {
        return;
      }

      onChange([...files, ...Array.from(event.currentTarget.files)]);
    },
    [files, onChange],
  );

export { useAdd };
export type { UseAdd };
