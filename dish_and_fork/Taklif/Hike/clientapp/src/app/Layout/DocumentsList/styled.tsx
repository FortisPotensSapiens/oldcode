import styled from '@emotion/styled';

const StyledLink = styled('a')({
  color: 'rgba(0, 0, 0, 0.3)',
  padding: '0px 1rem',
  fontSize: '60%',
  lineHeight: '120%',
});

const Container = styled('div')({
  display: 'grid',
  gridTemplateColumns: '1fr 1fr 1fr',
  rowGap: '0.2rem',
});

export { Container, StyledLink };
