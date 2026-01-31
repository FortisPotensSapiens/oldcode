import { useQuery, UseQueryResult } from 'react-query';

import { PartnerReadModel } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';
import { buildTimestampString } from './utils';

type UseGetMyPartnership = (refetchOnMount?: boolean, disableCache?: boolean) => UseQueryResult<PartnerReadModel>;

const useGetMyPartnership: UseGetMyPartnership = (refetchOnMount = false, disableCache = false) => {
  const { get } = useAxios();

  return useQuery(
    ['apiV1GetMyPartnership'],
    () => extractData(get(`/partners/my?${buildTimestampString(disableCache)}`)),
    { refetchOnMount },
  );
};

export { useGetMyPartnership };
export type { UseGetMyPartnership };
