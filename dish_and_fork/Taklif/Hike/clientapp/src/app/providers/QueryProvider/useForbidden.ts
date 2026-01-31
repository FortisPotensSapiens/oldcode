import { SnackbarMessage, useSnackbar } from 'notistack';
import { useCallback } from 'react';

type UseForbidden = (message?: SnackbarMessage, autoHideDuration?: number) => () => void;

const useForbidden: UseForbidden = (message = 'Доступ запрещeн.', autoHideDuration = 1000) => {
  const { enqueueSnackbar } = useSnackbar();

  return useCallback(() => {
    enqueueSnackbar(message, {
      autoHideDuration,
      variant: 'error',
    });
  }, [autoHideDuration, enqueueSnackbar, message]);
};

export { useForbidden };
export type { UseForbidden };
