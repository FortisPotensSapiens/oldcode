import { useQuery, UseQueryResult } from 'react-query';

import { ApplicationDetailsReadModel } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UseGetIndividualOrder = (orderId: string) => UseQueryResult<ApplicationDetailsReadModel>;

const useGetIndividualOrder: UseGetIndividualOrder = (orderId) => {
  const { get } = useAxios();

  return useQuery(['useGetIndividualOrder', orderId], () => extractData(get(`/individual-applications/${orderId}`)));
};

export { useGetIndividualOrder };
export type { UseGetIndividualOrder };
