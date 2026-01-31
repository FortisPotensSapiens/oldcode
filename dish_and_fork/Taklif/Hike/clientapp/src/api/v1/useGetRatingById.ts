import { useQuery, UseQueryResult } from 'react-query';

import { MerchRatingReadModel } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UseGetRatingById = (id: string) => UseQueryResult<MerchRatingReadModel>;

const useGetRatingById: UseGetRatingById = (id: string) => {
  const { get } = useAxios();

  return useQuery(['apiV1GetPartnerById', id], () => extractData(get(`merch-ratings/${id}`)));
};

export { useGetRatingById };
