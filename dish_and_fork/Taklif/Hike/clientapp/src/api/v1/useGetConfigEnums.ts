import { useQuery, UseQueryResult } from 'react-query';

import { EnumInfoModel } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UseGetConfigEnums = () => UseQueryResult<Record<string, EnumInfoModel[]>>;

export enum CONFIG_ENUM_KEYS {
  CategoryType = 'Hike.Entities.CategoryType',
}

const useGetConfigEnums: UseGetConfigEnums = () => {
  const { get } = useAxios();

  return useQuery(['apiV1ConfigEnumsList'], () => extractData(get(`/config/enums`)));
};

export { useGetConfigEnums };
