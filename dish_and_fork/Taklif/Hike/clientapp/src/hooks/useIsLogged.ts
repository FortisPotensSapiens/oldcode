import { useOidc } from '@axa-fr/react-oidc';

type UseIsLogged = () => boolean;

const useIsLogged: UseIsLogged = () => {
  const { isAuthenticated } = useOidc();

  return isAuthenticated;
};

export { useIsLogged };
