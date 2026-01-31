import { QueryKey, useQuery, UseQueryOptions, UseQueryResult } from 'react-query';

import { extractData } from '~/utils';

import { OfferDetailsReamModel } from '../../types/swagger';
import { useAxios } from './axios';

type UseGetIndividualOrderOfferInfo = (
  offerId: string,
  options?: Omit<
    UseQueryOptions<OfferDetailsReamModel, unknown, OfferDetailsReamModel, QueryKey>,
    'queryKey' | 'queryFn'
  >,
) => UseQueryResult<OfferDetailsReamModel>;

const useGetIndividualOrderOfferInfo: UseGetIndividualOrderOfferInfo = (offerId, options) => {
  const { get } = useAxios();

  return useQuery<OfferDetailsReamModel>(
    ['useGetIndividualOrderOfferInfo', offerId],
    () => extractData(get(`/individual-applications/offers/${offerId}`)),
    options,
  );
};

export { useGetIndividualOrderOfferInfo };
export type { UseGetIndividualOrderOfferInfo };
