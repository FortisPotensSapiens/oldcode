import { FC } from 'react';

import { Container, StyledLink } from './styled';
import { CONFIG_DOCUMENTS_TRANSLATIONS } from './types';

const DocumentsList: FC<{ documents: Record<string, string> }> = ({ documents }) => {
  return (
    <Container>
      {Object.keys(documents).map((item) => {
        return (
          <StyledLink target="_blank" href={documents[item]} key={item}>
            {CONFIG_DOCUMENTS_TRANSLATIONS[item]}
          </StyledLink>
        );
      })}
    </Container>
  );
};

export { DocumentsList };
