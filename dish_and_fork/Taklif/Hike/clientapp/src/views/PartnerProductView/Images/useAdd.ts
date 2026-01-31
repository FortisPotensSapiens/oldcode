import { useCallback } from 'react';

import { ChangeHandler, ImageValue } from './types';

type UseAdd = (value: ImageValue[], onChange: ChangeHandler) => (ids: string[]) => void;

const useAdd: UseAdd = (value, onChange) =>
  useCallback(
    (images) => {
      onChange([...value, ...images]);
    },
    [value, onChange],
  );

export { useAdd };
export type { UseAdd };
