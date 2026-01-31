import { useMutation, UseMutationResult } from 'react-query';

import { CategoryCreateModel } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UsePostAdminCategories = () => UseMutationResult<string, unknown, CategoryCreateModel, unknown>;

const usePostAdminCategories: UsePostAdminCategories = () => {
  const { post } = useAxios();

  return useMutation((data) => extractData(post('/admin/categories', data)));
};

export { usePostAdminCategories };
export type { UsePostAdminCategories };
