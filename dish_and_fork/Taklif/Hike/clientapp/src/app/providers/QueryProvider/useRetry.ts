import { useCallback } from 'react';

import { useConfig } from '~/contexts';
import { isAxiosError } from '~/utils';

type UseRetryOptions = { onForbidden?: () => void };

type UseRetry = (options?: UseRetryOptions) => (failureCount: number, error: unknown) => boolean;

const useRetry: UseRetry = ({ onForbidden } = {}) => {
  const { api } = useConfig();

  return useCallback(
    (failureCount: number, error: unknown) => {
      if (!isAxiosError(error)) {
        return false;
      }

      const status = error.response?.status;

      if (!status || status === 401) {
        return false;
      }

      if (status === 400) {
        console.error(error);
        return false;
      }

      if (status !== 403) {
        return failureCount < api.retries;
      }

      if (failureCount === 0) {
        return true;
      }

      onForbidden?.();

      return false;
    },
    [api.retries, onForbidden],
  );
};

export { useRetry };
export type { UseRetry, UseRetryOptions };
