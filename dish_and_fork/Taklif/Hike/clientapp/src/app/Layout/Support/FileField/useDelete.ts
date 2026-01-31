import { MouseEventHandler, useCallback } from 'react';

import { ChangeHandler } from './types';

type UseDelete = (files: File[], onChange: ChangeHandler, disabled?: boolean) => MouseEventHandler<HTMLButtonElement>;

const useDelete: UseDelete = (files, onChange, disabled) =>
  useCallback(
    (event) => {
      if (disabled) {
        return;
      }

      const dataIndex = event.currentTarget.getAttribute('data-index');

      if (!dataIndex) {
        return;
      }

      try {
        const changed = [...files];
        changed.splice(parseInt(dataIndex, 10), 1);

        onChange(changed);
      } catch (e) {
        console.error(`Can't delete file #${dataIndex} from form`);
      }
    },
    [disabled, files, onChange],
  );

export { useDelete };
export type { UseDelete };
