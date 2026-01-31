import { QueryKey, useQuery, UseQueryOptions, UseQueryResult } from 'react-query';

import { PartnerReadModel } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UseGetPartnerById = (
  id: string,
  options?: Omit<UseQueryOptions<unknown, unknown, PartnerReadModel, QueryKey>, 'queryKey' | 'queryFn'>,
) => UseQueryResult<PartnerReadModel>;

const useGetPartnerById: UseGetPartnerById = (id, options) => {
  const { get } = useAxios();

  return useQuery(['apiV1GetPartnerById', id], () => extractData(get(`/partners/${id}`)), options);
};

export { useGetPartnerById };
export type { UseGetPartnerById };
