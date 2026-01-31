import { useQuery, UseQueryResult } from 'react-query';

import { CollectionReadModel } from '~/types/collection';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UseGetAdminCollections = () => UseQueryResult<CollectionReadModel[]>;

const useGetAdminCollections: UseGetAdminCollections = () => {
  const { get } = useAxios();

  return useQuery(['useGetAdminPartners'], () => extractData(get(`/collections/admin`)));
};

export { useGetAdminCollections };
export type { UseGetAdminCollections };
