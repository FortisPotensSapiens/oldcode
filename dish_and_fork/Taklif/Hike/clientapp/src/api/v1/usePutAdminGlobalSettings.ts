import { useMutation, UseMutationResult } from 'react-query';

import { GlobaSettingsModel } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UsePutAdminGlobalSettings = () => UseMutationResult<string, unknown, GlobaSettingsModel, unknown>;

const usePutAdminGlobalSettings: UsePutAdminGlobalSettings = () => {
  const { put } = useAxios();

  return useMutation((data) => extractData(put('/config/admin/global-settings', data)));
};

export { usePutAdminGlobalSettings };
export type { UsePutAdminGlobalSettings };
