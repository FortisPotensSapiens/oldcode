import { AxiosError } from 'axios';
import { useSnackbar } from 'notistack';
import { useMemo } from 'react';
import { QueryClient } from 'react-query';

import { useRetry, UseRetryOptions } from './useRetry';

type UseQueryClient = (options?: UseRetryOptions) => QueryClient;

const useQueryClient: UseQueryClient = (options) => {
  const retry = useRetry(options);
  const { enqueueSnackbar } = useSnackbar();

  return useMemo(
    () =>
      new QueryClient({
        defaultOptions: {
          mutations: {
            retry,
            onError: (error) => {
              const err = error as AxiosError;
              let errorMessage = 'Неизвестная ошибка';

              if (err?.response?.data?.errors) {
                errorMessage = Object.values(err?.response?.data?.errors).join('\n');
              }

              if (err.response?.data?.message) {
                errorMessage = err.response?.data?.message;
              }

              enqueueSnackbar(errorMessage, {
                variant: 'error',
              });
            },
          },

          queries: {
            refetchOnWindowFocus: false,
            retry,
          },
        },
      }),
    [enqueueSnackbar, retry],
  );
};

export { useQueryClient };
export type { UseRetryOptions };
