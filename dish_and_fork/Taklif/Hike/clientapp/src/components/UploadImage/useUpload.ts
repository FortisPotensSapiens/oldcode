import { ChangeEventHandler, useCallback } from 'react';

import { usePostFile } from '~/api';

type UseUpload = (
  onComplete: (id: string) => void,
) => [ChangeEventHandler<HTMLInputElement>, { isError: boolean; isLoading: boolean; isSuccess: boolean }];

const useUpload: UseUpload = (onComplete) => {
  const { isError, isLoading, isSuccess, mutateAsync } = usePostFile();

  const handler = useCallback<ChangeEventHandler<HTMLInputElement>>(
    async (event) => {
      const file = event.target.files?.item(0);

      if (!file) {
        return;
      }

      const formData = new FormData();
      formData.append('file', file);

      try {
        onComplete(await mutateAsync(formData));
      } catch (e) {
        console.error(`Can't upload file ${file.name}`);
      }
    },
    [mutateAsync, onComplete],
  );

  return [handler, { isError, isLoading, isSuccess }];
};

export { useUpload };
export type { UseUpload };
