import { useMutation, UseMutationResult } from 'react-query';

import { CollectionUpdateModel } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UsePutAdminCollection = () => UseMutationResult<string, unknown, CollectionUpdateModel, unknown>;

const usePutAdminCollection: UsePutAdminCollection = () => {
  const { put } = useAxios();

  return useMutation((data) => extractData(put(`/collections/admin`, data)));
};

export { usePutAdminCollection };
export type { UsePutAdminCollection };
