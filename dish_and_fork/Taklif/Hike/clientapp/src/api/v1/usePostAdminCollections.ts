import { useMutation, UseMutationResult } from 'react-query';

import { CollectionCreateModel } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UsePostAdminCollections = () => UseMutationResult<CollectionCreateModel, unknown, unknown, unknown>;

const usePostAdminCollections: UsePostAdminCollections = () => {
  const { post } = useAxios();

  return useMutation((data) => extractData(post('/collections/admin', data)));
};

export { usePostAdminCollections };
export type { UsePostAdminCollections };
