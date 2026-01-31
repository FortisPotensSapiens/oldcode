import { useMutation, UseMutationResult } from 'react-query';

import { MerchandiseCreateModel } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UsePostNewProduct = () => UseMutationResult<string, unknown, MerchandiseCreateModel, unknown>;

const usePostNewProduct: UsePostNewProduct = () => {
  const { post } = useAxios();

  return useMutation((data) => extractData(post(`/seller/goods`, data)));
};

export { usePostNewProduct };
export type { UsePostNewProduct };
