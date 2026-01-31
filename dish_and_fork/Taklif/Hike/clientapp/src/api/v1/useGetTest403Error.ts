import { useQuery, UseQueryResult } from 'react-query';

import { ConfigModel } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UseGetTest403Error = (refetchOnMount?: boolean) => UseQueryResult<ConfigModel>;

const useGetTest403Error: UseGetTest403Error = () => {
  const { get } = useAxios();

  return useQuery(['apiV1GetTest403Error'], () => extractData(get(`/Test/Get403Error`)));
};

export { useGetTest403Error };
export type { UseGetTest403Error };
