import { useQuery, UseQueryOptions, UseQueryResult } from 'react-query';

import { UseQueryError } from '~/types';
import { extractData } from '~/utils';

import { CategoryFilterModel, CategoryReadModelPageResultModel } from '../../types/swagger';
import { useAxios } from './axios';

type UsePostCategoriesByFilter = (
  payload: CategoryFilterModel,
  options?: Omit<UseQueryOptions<CategoryReadModelPageResultModel, UseQueryError>, 'queryKey' | 'queryFn'>,
) => UseQueryResult<CategoryReadModelPageResultModel, UseQueryError>;

const usePostCategoriesFilter: UsePostCategoriesByFilter = (payload, options) => {
  const { post } = useAxios();

  return useQuery(
    ['apiV1useCategoriesFileter', payload],
    () => extractData(post<CategoryReadModelPageResultModel>('/categories/filter', payload)),
    options,
  );
};

export { usePostCategoriesFilter };
export type { UsePostCategoriesByFilter };
