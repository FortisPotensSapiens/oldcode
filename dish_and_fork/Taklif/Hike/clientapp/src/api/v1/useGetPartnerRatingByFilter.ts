import { useQuery, UseQueryResult } from 'react-query';

import { PartnerRatingFilterModel, PartnerRatingReadModelPageResultModel } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UseGetPartnerRatingByFilter = (
  payload: PartnerRatingFilterModel,
) => UseQueryResult<PartnerRatingReadModelPageResultModel>;

const useGetPartnerRatingByFilter: UseGetPartnerRatingByFilter = (payload) => {
  const { post } = useAxios();

  return useQuery(['useGetPartnerRatingByFilter', payload.pageNumber, payload.pageSize], () =>
    extractData(post<PartnerRatingReadModelPageResultModel>('partner-ratings/filter', payload)),
  );
};
export { useGetPartnerRatingByFilter };
export type { UseGetPartnerRatingByFilter };
