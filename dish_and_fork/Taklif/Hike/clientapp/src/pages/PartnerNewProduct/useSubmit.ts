import { useCallback } from 'react';
import { useNavigate } from 'react-router-dom';

import { usePostNewProduct } from '~/api';
import { generatePartnerProductPath, getPartnerProductsPath } from '~/routing';
import { MerchandiseCreateModel } from '~/types';

type SubmitHandler = (data: MerchandiseCreateModel, stay?: boolean) => Promise<void>;

type UseSubmit = () => [
  SubmitHandler,
  {
    isError: boolean;
    isLoading: boolean;
    isSuccess: boolean;
  },
];

const useSubmit: UseSubmit = () => {
  const navigate = useNavigate();
  const { isError, isLoading, isSuccess, mutateAsync } = usePostNewProduct();

  const handler = useCallback<SubmitHandler>(
    async (data, stay) => {
      try {
        const productId = await mutateAsync(data);

        navigate(stay ? generatePartnerProductPath({ productId }) : getPartnerProductsPath());
      } catch (e) {
        console.error(`Can't post new product:`, data);
      }
    },
    [mutateAsync, navigate],
  );

  return [handler, { isError, isLoading, isSuccess }];
};

export { useSubmit };
