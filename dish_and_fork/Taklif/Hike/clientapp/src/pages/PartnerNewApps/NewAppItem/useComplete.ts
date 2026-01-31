import { useCallback } from 'react';

import { SubmitCallback } from './FormDialog';

const useComplete = (onComplete?: SubmitCallback, doAfter?: () => void) =>
  useCallback(
    (id: string) => {
      onComplete?.(id);
      doAfter?.();
    },
    [onComplete, doAfter],
  );

export { useComplete };
