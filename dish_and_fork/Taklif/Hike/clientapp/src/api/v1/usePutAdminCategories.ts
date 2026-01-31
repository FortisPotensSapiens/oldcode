import { useMutation, UseMutationResult } from 'react-query';

import { CategoryUpdateModel } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UsePutAdminCategories = () => UseMutationResult<string, unknown, CategoryUpdateModel, unknown>;

const usePutAdminCategories: UsePutAdminCategories = () => {
  const { put } = useAxios();

  return useMutation((data) => extractData(put('/admin/categories', data)));
};

export { usePutAdminCategories };
export type { UsePutAdminCategories };
