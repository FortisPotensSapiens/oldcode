import { useMutation, UseMutationResult } from 'react-query';

import { extractData } from '~/utils';

import { useAxios } from './axios';

type UseDeleteGood = () => UseMutationResult<string, unknown, { goodId: string }>;

const useDeleteGood: UseDeleteGood = () => {
  const ax = useAxios();

  return useMutation((data) => extractData(ax.delete(`/seller/goods/${data.goodId}`)));
};

export { useDeleteGood };
export type { UseDeleteGood };
