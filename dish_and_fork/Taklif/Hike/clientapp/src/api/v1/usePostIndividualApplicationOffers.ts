import { useMutation, UseMutationResult } from 'react-query';

import { OfferCreateModel } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UsePostIndividualApplicationOffers = () => UseMutationResult<string, unknown, OfferCreateModel, unknown>;

const usePostIndividualApplicationOffers: UsePostIndividualApplicationOffers = () => {
  const { post } = useAxios();

  return useMutation((data) => extractData(post('/individual-applications/offers', data)));
};

export { usePostIndividualApplicationOffers };
export type { UsePostIndividualApplicationOffers };
