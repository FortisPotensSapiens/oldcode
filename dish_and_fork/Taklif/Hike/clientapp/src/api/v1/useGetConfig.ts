import { useQuery, UseQueryResult } from 'react-query';

import { ConfigModel } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UseGetConfig = (refetchOnMount?: boolean) => UseQueryResult<ConfigModel>;

const useGetConfig: UseGetConfig = (refetchOnMount = false) => {
  const { get } = useAxios();

  return useQuery(['apiV1GetConfig'], () => extractData(get(`/config`)), { refetchOnMount });
};

export { useGetConfig };
export type { UseGetConfig };
