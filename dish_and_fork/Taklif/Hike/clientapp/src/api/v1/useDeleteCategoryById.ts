import { useMutation, UseMutationResult } from 'react-query';

import { extractData } from '~/utils';

import { useAxios } from './axios';

type UseDeleteCategoryById = () => UseMutationResult<string, unknown, { categoryId: string }>;

const useDeleteCategoryById: UseDeleteCategoryById = () => {
  const ax = useAxios();

  return useMutation((data) => extractData(ax.delete(`/admin/categories/${data.categoryId}`)));
};

export { useDeleteCategoryById };
export type { UseDeleteCategoryById };
