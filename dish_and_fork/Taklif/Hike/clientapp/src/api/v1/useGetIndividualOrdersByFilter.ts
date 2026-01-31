import sq from 'query-string';
import { useQuery, UseQueryResult } from 'react-query';

import { ApplicationReadModelPageResultModel, PaginationModel } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UseGetIndividualOrdersByFilter = (payload: PaginationModel) => UseQueryResult<ApplicationReadModelPageResultModel>;

const useGetIndividualOrdersByFilter: UseGetIndividualOrdersByFilter = (payload) => {
  const { get } = useAxios();

  return useQuery(['useGetIndividualOrdersByFilter', payload.pageNumber, payload.pageSize], () =>
    extractData(get(`/individual-applications?${sq.stringify(payload)}`)),
  );
};

export { useGetIndividualOrdersByFilter };
export type { UseGetIndividualOrdersByFilter };
