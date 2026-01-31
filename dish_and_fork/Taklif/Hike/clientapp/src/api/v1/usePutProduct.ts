import { useMutation, UseMutationResult } from 'react-query';

import { MerchandiseCreateModel } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UsePutProduct = (id: string) => UseMutationResult<string, unknown, MerchandiseCreateModel, unknown>;

const usePutProduct: UsePutProduct = (id) => {
  const { put } = useAxios();

  return useMutation((data) => extractData(put(`/seller/goods`, { id, ...data })));
};

export { usePutProduct };
export type { UsePutProduct };
