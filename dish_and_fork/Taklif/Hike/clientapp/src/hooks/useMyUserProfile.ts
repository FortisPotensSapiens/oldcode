import { UseGetMyUserProfile, useGetMyUserProfile } from '~/api';

import { useIsLogged } from './useIsLogged';

type UseMyUserProfile = UseGetMyUserProfile;

const useMyUserProfile: UseGetMyUserProfile = (options = {}) => {
  const enabled = useIsLogged();
  const result = useGetMyUserProfile({ enabled, refetchOnMount: false, ...options });

  return result;
};

export { useMyUserProfile };
export type { UseMyUserProfile };
