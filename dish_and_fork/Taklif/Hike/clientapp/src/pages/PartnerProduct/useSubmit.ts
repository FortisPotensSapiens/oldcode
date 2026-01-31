import { useCallback } from 'react';
import { useNavigate } from 'react-router-dom';

import { usePutProduct } from '~/api';
import { getPartnerProductsPath } from '~/routing';
import { MerchandiseCreateModel } from '~/types';

type SubmitHandler = (data: MerchandiseCreateModel, stay?: boolean) => Promise<void>;

type UseSubmit = (productId: string) => [
  SubmitHandler,
  {
    isError: boolean;
    isLoading: boolean;
    isSuccess: boolean;
  },
];

const useSubmit: UseSubmit = (productId) => {
  const navigate = useNavigate();
  const { isError, isLoading, isSuccess, mutateAsync } = usePutProduct(productId);

  const handler = useCallback<SubmitHandler>(
    async (data, stay) => {
      try {
        await mutateAsync(data);

        if (!stay) {
          navigate(-1);
        }
      } catch (e) {
        console.error(`Can't patch product "${productId}":`, data);
      }
    },
    [mutateAsync, navigate, productId],
  );

  return [handler, { isError, isLoading, isSuccess }];
};

export { useSubmit };
