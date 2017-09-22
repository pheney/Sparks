function IsNullOrWhiteSpace(source) {
    return (typeof source === 'undefined')
    || (source == null)
    || (source.toString().trim().length == 0);
}