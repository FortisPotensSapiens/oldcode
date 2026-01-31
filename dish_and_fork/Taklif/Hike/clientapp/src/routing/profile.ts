import { joinPath } from '~/utils';

const PROFILE_PART = 'profile';

const getProfilePath = (): string => joinPath(PROFILE_PART);

export { getProfilePath, PROFILE_PART };
