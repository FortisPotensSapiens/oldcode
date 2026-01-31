import { useQuery, UseQueryResult } from 'react-query';

import { PartnerReadModel } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UseGetPartnerMy = () => UseQueryResult<PartnerReadModel>;

const useGetPartnerMy: UseGetPartnerMy = () => {
  const { get } = useAxios();

  return useQuery('useGetPartnerMy', () => extractData(get(`/partners/my?timestamp=${Date.now()}`)));
};

export { useGetPartnerMy };
export type { UseGetPartnerMy };
