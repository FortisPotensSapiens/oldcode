import { ChangeEventHandler, useCallback, useEffect } from 'react';

import { usePostFile } from '~/api';

type UseChange = (
  onChange: (value?: string | undefined) => void,
  onUploading: (value: boolean) => void,
) => [ChangeEventHandler<HTMLInputElement>, { isLoading: boolean }];

const useChange: UseChange = (onChange, onUploading) => {
  const { isLoading, mutateAsync } = usePostFile();

  useEffect(() => {
    if (onUploading) {
      onUploading(isLoading);
    }
  }, [onUploading, isLoading]);

  const handler = useCallback<ChangeEventHandler<HTMLInputElement>>(
    async (event) => {
      const file = event.currentTarget.files?.item(0);

      if (!file) {
        return;
      }

      const formData = new FormData();
      formData.append('file', file);

      try {
        const result = await mutateAsync(formData);

        if (onChange) {
          onChange(result);
        }
      } catch (e) {
        console.error(`Can't upload file:`, file);
      }
    },
    [mutateAsync, onChange],
  );

  return [handler, { isLoading }];
};

export { useChange };
export type { UseChange };
