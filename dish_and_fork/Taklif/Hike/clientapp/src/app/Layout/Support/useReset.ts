import { useEffect } from 'react';

type UseReset = (condition: boolean, action: () => void) => void;

const useReset: UseReset = (condition, action) => {
  useEffect(() => {
    if (condition) {
      action();
    }
  }, [condition, action]);
};

export { useReset };
export type { UseReset };
