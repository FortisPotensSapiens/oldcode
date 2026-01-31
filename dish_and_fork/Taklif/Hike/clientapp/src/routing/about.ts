import { joinPath } from '~/utils';

const ABOUT_PART = 'about';

const getAboutPath = (): string => joinPath(ABOUT_PART);

export { ABOUT_PART, getAboutPath };
