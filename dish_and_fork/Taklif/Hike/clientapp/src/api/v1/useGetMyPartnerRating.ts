import { useQuery, UseQueryResult } from 'react-query';

import { MerchRatingReadModel } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UseGetMyPartnerRating = (id: string) => UseQueryResult<MerchRatingReadModel>;

const useGetMyPartnerRating: UseGetMyPartnerRating = (id: string) => {
  const { get } = useAxios();

  return useQuery(['UseGetMyPartnerRating', id], () => extractData(get(`my-assigned-partner-rating/${id}`)));
};

export { useGetMyPartnerRating };
export type { UseGetMyPartnerRating };
