import styled from '@emotion/styled';

const Container = styled.div`
  font-size: 60%;
  color: rgba(0, 0, 0, 0.3);
  padding-right: 1rem;
  white-space: nowrap;

  a {
    color: rgba(0, 0, 0, 0.3);
  }
`;

const Contacts = () => {
  return (
    <Container>
      <strong>Контакты:</strong>
      <br />
      ООО &laquo;ДИШ ЭНД ФОРК&raquo;
      <br />
      ИНН 5904404704
      <br />
      <a href="mailto:info@dishfork.ru">info@dishfork.ru</a>
    </Container>
  );
};

export { Contacts };
