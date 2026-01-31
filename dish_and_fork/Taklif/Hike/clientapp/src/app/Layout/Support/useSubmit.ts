import { useTheme } from '@mui/material';
import { useCallback, useEffect } from 'react';
import { UseFormReset } from 'react-hook-form';

import { usePostSupportMessage } from '~/api';

import { SupportFormData } from './types';

type SubmitHandler = (data: SupportFormData) => Promise<void>;

type UseSubmit = (
  resetCondition: boolean,
  resetForm: UseFormReset<SupportFormData>,
  resetFiles: (files: File[]) => void,
  files: File[],
) => [
  SubmitHandler,
  {
    isError: boolean;
    isLoading: boolean;
    isSuccess: boolean;
  },
];

const useSubmit: UseSubmit = (resetCondition, resetForm, resetFiles, files) => {
  const { isError, isLoading, isSuccess, mutateAsync, reset } = usePostSupportMessage();
  const { transitions } = useTheme();

  const call = useCallback<SubmitHandler>(
    async ({ email, message, name = '' }) => {
      try {
        const formData = new FormData();

        formData.set('Email', email);
        formData.set('Name', name);
        formData.set('Message', message);

        files.forEach((file) => formData.append('Files', file));

        await mutateAsync(formData);
        resetForm({ email: '', message: '', name: '' });
        resetFiles([]);
      } catch (e) {
        console.error("Can't send message to support", e);
      }
    },
    [files, mutateAsync, resetFiles, resetForm],
  );

  useEffect(() => {
    if (!resetCondition) {
      return () => undefined;
    }

    const timer = setTimeout(() => reset(), transitions.duration.leavingScreen);

    return () => clearTimeout(timer);
  }, [resetCondition, reset, transitions.duration.leavingScreen]);

  return [call, { isError, isLoading, isSuccess }];
};

export { useSubmit };
