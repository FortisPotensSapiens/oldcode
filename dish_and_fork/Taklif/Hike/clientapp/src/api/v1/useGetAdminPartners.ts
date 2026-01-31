import { useQuery, UseQueryResult } from 'react-query';

import { PartnerReadModel } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UseGetAdminPartners = () => UseQueryResult<PartnerReadModel[]>;

const useGetAdminPartners: UseGetAdminPartners = () => {
  const { get } = useAxios();

  return useQuery(['useGetAdminPartners'], () => extractData(get(`/admim/partners`)));
};

export { useGetAdminPartners };
export type { UseGetAdminPartners };
