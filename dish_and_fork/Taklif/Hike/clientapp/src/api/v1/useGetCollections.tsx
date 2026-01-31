import { useQuery, UseQueryResult } from 'react-query';

import { CollectionReadModel } from '~/types/collection';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UseGetCollections = () => UseQueryResult<CollectionReadModel[]>;

const useGetCollections: UseGetCollections = () => {
  const { get } = useAxios();

  return useQuery(['useGetCollections'], () => extractData(get(`/collections/`)));
};

export { useGetCollections };
export type { UseGetCollections };
