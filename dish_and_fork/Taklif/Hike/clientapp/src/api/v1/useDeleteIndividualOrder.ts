import { useMutation, UseMutationResult } from 'react-query';

import { extractData } from '~/utils';

import { useAxios } from './axios';

type UseDeleteIndividualOrder = () => UseMutationResult<string, unknown, { orderId: string }>;

const useDeleteIndividualOrder: UseDeleteIndividualOrder = () => {
  const ax = useAxios();

  return useMutation((data) => extractData(ax.delete(`/individual-applications/${data.orderId}`)));
};

export { useDeleteIndividualOrder };
export type { UseDeleteIndividualOrder };
