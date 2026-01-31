import { useMutation, UseMutationResult } from 'react-query';

import { extractData } from '~/utils';

import { useAxios } from './axios';

type UseDeleteIndividualOrderOffer = () => UseMutationResult<string, unknown, { offerId: string }>;

const useDeleteIndividualOrderOffer: UseDeleteIndividualOrderOffer = () => {
  const ax = useAxios();

  return useMutation((data) => extractData(ax.delete(`/seller/individual-applications/offers/${data.offerId}`)));
};

export { useDeleteIndividualOrderOffer };
export type { UseDeleteIndividualOrderOffer };
