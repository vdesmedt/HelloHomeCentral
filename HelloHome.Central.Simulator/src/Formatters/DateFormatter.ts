const DTFormatter = new Intl.DateTimeFormat('en-BE', {
    weekday: 'short',
    year: '2-digit',
    month: 'short',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
    second: '2-digit',
});

const formatDate : (date:Date|undefined) => string = (date: Date|undefined) => date == undefined ? "Null Date": DTFormatter.format(date);
const formatDateString : (date:string) => string = (date: string) => DTFormatter.format(new Date(date));

export { formatDate, formatDateString }