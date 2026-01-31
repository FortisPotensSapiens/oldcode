import { useQuery, UseQueryResult } from 'react-query';

import { OfferSellerReadModelPageResultModel } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UseGetSellerIndividualApplicationOffersMyFilter = (
  pageSize: number,
  pageNumber: number,
) => UseQueryResult<OfferSellerReadModelPageResultModel>;

const useGetSellerIndividualApplicationOffersMyFilter: UseGetSellerIndividualApplicationOffersMyFilter = (
  pageSize,
  pageNumber,
) => {
  const { post } = useAxios();

  return useQuery(['apiV1GetSellerIndividualApplications', pageNumber, pageSize], () =>
    extractData(post('/seller/individual-applications/offers/my/filter', { pageNumber, pageSize })),
  );
};
export { useGetSellerIndividualApplicationOffersMyFilter };
export type { UseGetSellerIndividualApplicationOffersMyFilter };
