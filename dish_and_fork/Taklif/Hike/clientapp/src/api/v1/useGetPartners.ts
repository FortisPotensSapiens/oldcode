import { useQuery, UseQueryResult } from 'react-query';

import { PartnerReadModel } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UseGetPartners = (key: string) => UseQueryResult<PartnerReadModel[]>;

const useGetPartners: UseGetPartners = (key) => {
  const { get } = useAxios();

  return useQuery([key], () => extractData(get(`/partners`)));
};

export { useGetPartners };
export type { UseGetPartners };
