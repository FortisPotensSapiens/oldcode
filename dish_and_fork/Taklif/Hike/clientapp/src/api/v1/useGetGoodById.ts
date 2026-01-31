import { useQuery, UseQueryResult } from 'react-query';

import { MerchandiseReadModel } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UseGetGoodById = (id: string) => UseQueryResult<MerchandiseReadModel>;

const useGetGoodById: UseGetGoodById = (id) => {
  const { get } = useAxios();

  return useQuery(['apiV1GetGoodById', id], () => extractData(get(`/goods/${id}`)));
};

export { useGetGoodById };
export type { UseGetGoodById };
