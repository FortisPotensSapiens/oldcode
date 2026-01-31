import { useQuery, UseQueryResult } from 'react-query';
import { createSearchParams } from 'react-router-dom';

import { ApplicationReadModelPageResultModel } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UseGetSellerIndividualApplications = (
  pageNumber: number,
  pageSize: number,
) => UseQueryResult<ApplicationReadModelPageResultModel>;

const useGetSellerIndividualApplications: UseGetSellerIndividualApplications = (pageN, pageS) => {
  const { get } = useAxios();
  const pageNumber = String(pageN);
  const pageSize = String(pageS);

  return useQuery(['apiV1GetSellerIndividualApplications', pageNumber, pageSize], () =>
    extractData(get(`/seller/individual-applications?${createSearchParams({ pageNumber, pageSize })}`)),
  );
};
export { useGetSellerIndividualApplications };
export type { UseGetSellerIndividualApplications };
